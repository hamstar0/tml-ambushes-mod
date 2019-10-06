using Terraria.ModLoader;


namespace Ambushes {
	public class AmbushesMod : Mod {
		public static AmbushesMod Instance { get; private set; }

		////////////////


		public static string GithubUserName => "hamstar0";
		public static string GithubProjectName => "tml-ambushes-mod";



		////////////////

		public AmbushesConfig Config => this.GetConfig<AmbushesConfig>();



		////////////////

		public AmbushesMod() {
			AmbushesMod.Instance = this;
		}

		public override void Load() {
		}

		public override void PostSetupContent() {
		}

		public override void Unload() {
			AmbushesMod.Instance = null;
		}
	}
}