﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIAutores.Entities
{
    public class Libro
    {
        public int Id { get; set; }
        public string Tittle { get; set; }
        public int AutorId { get; set; }
        public Autor Autor { get; set; }
    }
}
