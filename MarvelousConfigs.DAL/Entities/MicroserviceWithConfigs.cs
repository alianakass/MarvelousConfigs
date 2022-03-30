﻿using MarvelousConfigs.DAL.Entities;

namespace MarvelousConfigs.DAL
{
    public class MicroserviceWithConfigs
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public string URL { get; set; }
        public List<Config> Configs { get; set; }
    }
}
