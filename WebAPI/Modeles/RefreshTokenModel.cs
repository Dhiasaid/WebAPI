namespace WebAPI.Modeles
{
    public class RefreshTokenModel
    {
        public string Token { get; set; }  
        public  string RefreshToken { get; set; }
        public  DateTime? Expiration { get; set; }
    }
}
