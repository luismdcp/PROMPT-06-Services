
namespace GitHubSoap.Domain.Repos
{
    public class BaseRepo
    {
        #region Properties

        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public bool has_issues { get; set; }
        public bool has_wiki { get; set; }
        public bool has_downloads { get; set; }

        #endregion
    }
}