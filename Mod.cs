using Colossal.Logging;
using Game;
using Game.Modding;
using Game.SceneFlow;
using System;

namespace ZoneDemerger
{
    public class Mod : IMod
    {
        public const string ModName = "Zone Demerger";
        public ZoneDemerger _zoneDemerger;


        public static ILog log = LogManager.GetLogger($"{nameof(ZoneDemerger)}.{nameof(Mod)}").SetShowsErrorsInUI(false);

        public void OnLoad(UpdateSystem updateSystem)
        {
            var startTime = DateTime.Now;
            log.Info(nameof(OnLoad));

            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
                log.Info($"Current mod asset at {asset.path}");

            _zoneDemerger = new ZoneDemerger();
            updateSystem.UpdateAfter<ZoneDemerger>(SystemUpdatePhase.PrefabUpdate);
            var loadTime = DateTime.Now - startTime;
            log.Info("Load Time: " + loadTime.TotalSeconds + "s");
        }

        public void OnDispose()
        {
            log.Info(nameof(OnDispose));
        }
    }
}
