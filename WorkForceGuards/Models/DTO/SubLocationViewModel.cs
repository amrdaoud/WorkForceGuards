using WorkForceManagementV0.Models;

namespace WorkForceGuards.Models.DTO
{
    public class SubLocationViewModel
    {
        public SubLocationViewModel(SubLocation model)
        {
            Id = model.Id;
            Name = model.Name;
            LocationId = model.LocationId;
            LocationName = model.Location.Name;
        }
        public int Id { get; set; }
        public string Name { get;set; }
        public string LocationName { get; set; }
        public int LocationId { get; set; }
    }
}
