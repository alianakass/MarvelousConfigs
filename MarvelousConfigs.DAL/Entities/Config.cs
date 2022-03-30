namespace MarvelousConfigs.DAL.Entities
{
    public class Config
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public int ServiceId { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

    }
}
