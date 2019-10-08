using Terraria.ModLoader;
using Terraria;
using HamstarHelpers.Helpers.DotNET.Extensions;
using HamstarHelpers.Helpers.HUD;
using HamstarHelpers.Services.Hooks.LoadHooks;
using HamstarHelpers.Helpers.Debug;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;


namespace Ambushes {
	public class AmbushesMod : Mod {
		public static AmbushesMod Instance { get; private set; }

		////////////////


		public static string GithubUserName => "hamstar0";
		public static string GithubProjectName => "tml-ambushes-mod";



		////////////////

		public AmbushesConfig Config => ModContent.GetInstance<AmbushesConfig>();



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
			if( !this.Config.DebugModeInfoMap ) { return; }

			var myworld = ModContent.GetInstance<AmbushesWorld>();

			if( Monitor.TryEnter(AmbushesWorld.MyLock) ) {
				foreach( (int tileX, IDictionary<int, Ambush> ambYs) in myworld.Ambushes ) {
					foreach( (int tileY, Ambush ambush) in ambYs ) {
						this.DrawAmbushOnFullscreenMap( tileX, tileY, ambush );
					}
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