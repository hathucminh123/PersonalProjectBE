namespace SalesProject.Models.Domain
{
    public class FavoriteProducts
    {

        public Guid Id {get; set;}

        public Guid UserId { get; set; }

        public Users? User { get; set; } // Một FavoriteProducts thuộc về một User  


        public Guid ProductId { get; set; }

        public Products? Product { get; set; } // Một FavoriteProducts thuộc về một Product


    }
}
