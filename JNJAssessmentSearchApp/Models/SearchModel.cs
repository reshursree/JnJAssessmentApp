namespace JNJAssessmentSearchApp.Models
{
    public class SearchModel
    {
        public string Term { get; set; }

        public string Type { get; set; }

        public List<PersonModel>? Persons { get; set; }
    }
}