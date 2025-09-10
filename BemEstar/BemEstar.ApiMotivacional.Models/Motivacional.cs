namespace BemEstar.ApiMotivacional.Models
{
    public class Motivacional : BaseModel
    {
        public string Texto { get; set; } = string.Empty;
        public string Autor {  get; set; } = string.Empty;

    }
}
