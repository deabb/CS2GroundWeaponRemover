using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;

namespace GroundWeaponRemover
{
    [MinimumApiVersion(171)]
    public partial class GroundWeaponRemover : BasePlugin
    {
        public override string ModuleName => "GroundWeaponRemover";
        public override string ModuleVersion => $"1.0 - {new DateTime(Builtin.CompileTime, DateTimeKind.Utc)}";
        public override string ModuleAuthor => "Deana https://x.com/dea_bb/";
        public override string ModuleDescription => "A Plugin to remove dropped weapons";

        private CounterStrikeSharp.API.Modules.Timers.Timer? weaponTimer;

        public override void Load(bool hotReload)
        {
            RegisterEventHandler<EventRoundStart>((@event, info) =>
            {
                StartWeaponTimer();
                return HookResult.Continue;
            });

            Console.WriteLine("[GroundWeaponRemover] Plugin Loaded");
        }

        private void StartWeaponTimer()
        {
            weaponTimer?.Kill();
            Console.WriteLine("[GroundWeaponRemover] Previous Weapon Timer Removed!");

            weaponTimer = AddTimer(1.0f, RemoveWeaponsOnTheGround);
            Console.WriteLine("[GroundWeaponRemover] Weapon Timer Started!");
        }

        private static void RemoveWeaponsOnTheGround()
        {
            var entities = Utilities.FindAllEntitiesByDesignerName<CCSWeaponBaseGun>("weapon_*");

            foreach (var entity in entities)
            {
                if (entity.IsValid && entity.State == CSWeaponState_t.WEAPON_NOT_CARRIED)
                {
                    entity.Remove();
                }
            }
        }
    }
}
