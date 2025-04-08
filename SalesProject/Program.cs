using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SalesProject.Data;
using System.Text;

using NZWalks.API.Data;
using SalesProject.Interface;
using SalesProject.Repositories;
using SalesProject.Mappings;
using SalesProject.Repository;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1",new OpenApiInfo { Title="SalesProjectAPI",Version="v1"});
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name ="Authorization",
        In =ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,   
        Scheme =JwtBearerDefaults.AuthenticationScheme

    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference =new OpenApiReference
                {
                    Type =ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme ="Oauth2",
                Name =JwtBearerDefaults.AuthenticationScheme,
                In =ParameterLocation.Header
            },

            new List<string>()
        }
    });

});
 



builder.Services.AddDbContext<SalesDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("SalesConnectionString")));

builder.Services.AddDbContext<AuthDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("AuthConnectionString")));

builder.Services.AddScoped<ICartRepository, CartService>();
builder.Services.AddScoped<IOrdersRepository, OrderService>();
builder.Services.AddScoped<IPaymentRespository, PaymentService>();
builder.Services.AddScoped<IDiscountRepository, DiscountService>();
builder.Services.AddScoped<IUserRepository, UsersService>();
builder.Services.AddScoped<ICompareRepository, CompareRepository>();
builder.Services.AddScoped<IProductRepository, ProductService>();
builder.Services.AddScoped<ISubCategoryRepository, SubCategoryService>();
builder.Services.AddScoped<ICategory, CategoryService>();
builder.Services.AddScoped<IEmailRepository, EmailService>();
builder.Services.AddScoped<IAddressRepository, AddressService>();

builder.Services.AddScoped<IVnPayService, VnPayService>();
builder.Services.AddScoped<IFavoriteProduct, FavoriteProductService>();
builder.Services.AddScoped<IBlogCategoryService, BlogCategoryService>();
builder.Services.AddScoped<IBlogSubCategoryService, BlogSubCategoryService>();
builder.Services.AddScoped<IBlogPostService, BlogPostService>();




builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

builder.Services.AddControllers();


builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;


});


    builder.Services.AddIdentityCore<IdentityUser>()
        .AddRoles<IdentityRole>()
        .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("SalesProject")
        .AddEntityFrameworkStores<AuthDbContext>()
        .AddDefaultTokenProviders();


// Cấu hình Authentication Google
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    byte[] key = Encoding.ASCII.GetBytes("QW12ER34TY56Ui78oi98v2bNh78JK4Hods7uUji12");

    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        AuthenticationType = "Jwt",
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
})
.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
{
    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.ClientId = builder.Configuration["GoogleAuth:ClientId"];
    options.ClientSecret = builder.Configuration["GoogleAuth:ClientSecret"];
});


builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;

}).AddEntityFrameworkStores<SalesDbContext>();
 
builder.Services.AddAuthorization();
builder.Services.AddControllers();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") // ✅ FE URL
                  .AllowAnyHeader()
                  .AllowAnyMethod()
             .AllowAnyOrigin();
        });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowSpecificOrigin");
app.UseHttpsRedirection(); 
app.UseAuthentication(); 
app.UseAuthorization();


app.UseRouting();

app.UseCors("AllowAll");
app.MapControllers();

app.Run();
