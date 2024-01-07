using Marina.BusinessLogic.Accounts;

namespace Marina.UI.Models
{
    public static class Extensions
    {
        public static LoginDto ToDto(this LoginVm model) => new(model.Username, model.Password);

        public static RegisterDto ToDto(this RegisterVm model) => new(model.UserName, model.LineId, model.ProvinceId, model.Password, model.DistributorName, model.RegionId, model.RSMId, model.DistributorId, model.PhoneNumber, model.SupervisorId, model.NsmId);
    }

}
