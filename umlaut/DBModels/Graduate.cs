namespace DBModels
{
    public class Graduate
    {
        public int Id { get; set; }

        public string? Gender { get; set; }

        public int Age { get; set; }

        public string? Vacation { get; set; }

        //добавить список специализаций

        public int ExpectedSalary { get; set; }

        public string? Location { get; set; }
        //возможно потребуется отдельная сущность

        public string? YearGraduation { get; set; }

        public string? Faculty { get; set; }
        //сами знаете

        public int Experience { get; set; }

        public string? ResumeLink { get; set; }



    }
}