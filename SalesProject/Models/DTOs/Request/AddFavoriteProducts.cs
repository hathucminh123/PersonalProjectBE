namespace SalesProject.Models.DTOs.Request
{
    public class AddFavoriteProducts
    {

        public Guid UserId { get; set; }


        public Guid ProductId { get; set; }
    }
}
