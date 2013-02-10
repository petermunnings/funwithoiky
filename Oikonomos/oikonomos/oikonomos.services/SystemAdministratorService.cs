using oikonomos.common;
using oikonomos.data;
using oikonomos.repositories.interfaces;
using oikonomos.services.interfaces;

namespace oikonomos.services
{
    public class SystemAdministratorService : ISystemAdministratorService
    {
        private readonly IChurchRepository _churchRepository;
        private readonly IPermissionRepository _permissionRepository;

        public SystemAdministratorService(IChurchRepository churchRepository, IPermissionRepository permissionRepository)
        {
            _churchRepository = churchRepository;
            _permissionRepository = permissionRepository;
        }

        public Church SetNewChurch(Person currentPerson, int churchId)
        {
            if (currentPerson.HasPermission(Permissions.SystemAdministrator))
            {
                var church = _churchRepository.GetChurch(churchId);
                _permissionRepository.SetupPermissions(currentPerson, church, true);
                return church;
            }
            return null;
        }
    }
}