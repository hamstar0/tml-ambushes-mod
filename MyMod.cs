using Terraria.ModLoader;
using Terraria;
using HamstarHelpers.Services.Hooks.LoadHooks;
using HamstarHelpers.Helpers.Debug;


namespace Ambushes {
	public partial class AmbushesMod : Mod {
		public static string GithubUserName => "hamstar0";
		public static string GithubProjectName => "tml-ambushes-mod";


		////////////////

		public static AmbushesMod Instance { get; private set; }

		public static AmbushesConfig Config => ModContent.GetInstance<AmbushesConfig>();



		////////////////

		public AmbushesMod() {
			AmbushesMod.Instance = this;
		}

		public override void Load() {
			Ambush.OnModsLoad();
		}

		public override void Unload() {
			Ambush.OnModsUnload();
			AmbushesMod.Instance = null;
		}

		////

		public override void PostSetupContent() {
			LoadHooks.AddPostWorldLoadEachHook( () => {
				if( Main.netMode != 1 ) {
					var myworld = ModContent.GetInstance<AmbushesWorld>();
					myworld.InitializeAmbushesAsync( myworld.MaxAmbushes );
				}
			} );
		}
	}
}