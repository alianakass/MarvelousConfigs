﻿using MarvelousConfigs.DAL.Entities;
using System.Collections;

namespace MarvelousConfigs.BLL.Tests
{
    public class DeleteOrRestoreMicroserviceByIdTestCaseSource : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            Microservice service = new Microservice()
            {
                Id = 1,
                ServiceName = "Name1",
                URL = "URL1"
            };

            int id = 1;

            yield return new object[] { id, service };
        }
    }
}