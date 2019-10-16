using Terraria.ModLoader;
using Terraria;
using HamstarHelpers.Helpers.HUD;
using HamstarHelpers.Services.Hooks.LoadHooks;
using HamstarHelpers.Helpers.Debug;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;


namespace Ambushes {
	public class AmbushesMod : Mod {
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
		}

		public override void PostSetupContent() {
			LoadHooks.AddPostWorldLoadEachHook( () => {
				if( Main.netMode != 1 ) {
					var myworld = ModContent.GetInstance<AmbushesWorld>();
					myworld.InitializeAmbushesAsync( myworld.MaxAmbushes );
				}
			} );
		}

		public override void Unload() {
			AmbushesMod.Instance = null;
		}


		////////////////

		public override void PostDrawFullscreenMap( ref string mouseText ) {
			if( !AmbushesMod.Config.DebugModeInfoMap ) { return; }

			var myworld = ModContent.GetInstance<AmbushesWorld>();

			if( !AmbushManager.IsLocked ) {
				foreach( Ambush ambush in myworld.AmbushMngr.GetAllAmbushes() ) {
					this.DrawAmbushOnFullscreenMap( ambush.TileX, ambush.TileY, ambush );
				}
			}
		}

		////

		private void DrawAmbushOnFullscreenMap( int tileX, int tileY, Ambush ambush ) {
			var wldPos = new Vector2( tileX * 16, tileY * 16 );
			var overMapData = HUDMapHelpers.GetFullMapScreenPosition( wldPos );

			if( overMapData.Item2 ) {
				Main.spriteBatch.DrawString(
					Main.fontMouseText,
					"X",
					overMapData.Item1,
					Color.Red
				);
			}
		}
	}
}