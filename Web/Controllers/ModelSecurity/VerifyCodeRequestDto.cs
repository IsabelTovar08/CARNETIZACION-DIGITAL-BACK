namespace Web.Controllers.ModelSecurity
{
    public class VerifyCodeRequestDto
    {
        public int UserId { get; set; }
        public string Code { get; set; } = default!;
    }
}
