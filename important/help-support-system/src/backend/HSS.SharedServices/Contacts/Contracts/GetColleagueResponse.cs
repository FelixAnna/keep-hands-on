namespace HSS.SharedServices.Contacts.Contracts
{
    public class GetColleagueResponse
    {
        public string UserId { get; set; } = null!;
        public IEnumerable<ColleagueModel> Colleagues { get; set; } = null!;
    }
}