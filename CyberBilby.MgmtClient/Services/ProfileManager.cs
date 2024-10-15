using CyberBilby.Shared.Network;

namespace CyberBilby.MgmtClient.Services;

public class ProfileManager
{
    public AuthProfile? CurrentProfile { get; private set; }

    public void UpdateProfile(AuthProfile profile)
    {
        CurrentProfile = profile;
    }
}
