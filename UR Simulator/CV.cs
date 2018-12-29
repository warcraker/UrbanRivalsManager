using System; using System.Collections.Generic;

namespace GuillermoMestreCarrion
{
    public class CurriculumVitae
    {
        public readonly int    Telefono  = 606881912;
        public readonly string Correo    = "gmcarrion@gmail.com";
        public string          Domicilio = "C/San Rafael, 58, izq, 1-2";
        public string          Poblacion = "Gandía (Valencia) 46701";

        public List<object> Formacion() {
            var Titulacion = new Tuple<string, string, string, string>
                ("Ingeniero Técnico en Informática de Gestión", "2005/06 - 2008/09",
                "Universidad Politécnica de Valencia", 
                "Nota media: 7,2. Especialización en Administración de Sistemas y Redes.");
            var Erasmus = new Tuple<string, string, string, string>
                ("Bachelor of Engineering in Computer Engineering (Year 3)", "2008/09",
                "Athlone Institute of Technology", 
                "Distinction Grade. Dissertation (Proyecto Final de Carrera).");
            return new List<object> { Titulacion, Erasmus };
        }

        public List<string> Habilidades = new List<string>() {
            "C# .Net Framework 4.5", "Java", "SQLite" };

        public string Idiomas(string idioma) {
            switch (idioma) {
                case "English":
                    return "High level. B2 EOI Certification. First Certificate Cambridge."
                        + "Stay of 10 months in Ireland as Erasmus student.";
                case "Valencià":
                    return "Bilingüe. Llengua materna.";
                case "Castellano":
                    return "Bilingüe. Lengua materna.";
                default:
                    return "No conocido";
        }   }

        public Dictionary<string, List<string>> Experiencia = new Dictionary<string, List<string>>() {
            { "Everis", new List<string>()
                { "Desde marzo 2015 hasta la actualidad",
                  "Centers Developer",
                  "Robot Process Automation",
                  "Consultoría software",
                } },
            { "Computer Aided Education", new List<string>()
                { "Desde julio 2009 hasta julio 2011", 
                  "Elaboración de cursos e-learning",
                  "Corrección de errores",
                  "Mantenimiento de manuales",
                } },
            { "Autónomo", new List<string>() 
                { "Desde noviembre 2011 hasta la actualidad", 
                  "Asistencia y servicio técnico a domicilio",
                } }
        };

        public static string Otros { 
            get { return "Permiso B2" +
                         "Disponiblidad inmediata" +
                         "Movilidad geográfica"; } }
    }
}
