using Developer_Toolbox.Models;
using System.Collections.Generic;

namespace Developer_Toolbox.Repositories
{
    public interface IBadgeRepository
    {
        IEnumerable<Badge> GetAllBadges();
        Badge GetBadgeById(int? id);
    }
}

