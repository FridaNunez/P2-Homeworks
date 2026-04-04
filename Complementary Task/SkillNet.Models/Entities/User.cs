namespace SkillNet.Models.Entities
{
    public class User
    {
        public int UserId { get; set; } // Asegúrate de que coincida con tu DB
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int RankId { get; set; }
    }
}