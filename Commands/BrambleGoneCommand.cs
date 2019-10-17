using Ambushes.Tiles;
using HamstarHelpers.Helpers.Tiles;
using System;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;


namespace Ambushes.Commands {
	public class BrambleGoneCommand : ModCommand {
		private static object MyLock = new object();



		////////////////

		public override CommandType Type {
			get {
				if( Main.netMode == 0 && !Main.dedServ ) {
					return CommandType.World;
				}
				return CommandType.Console | CommandType.World;
			}
		}
		public override string Command => "amb-bramblegone";
		public override string Usage => "/" + this.Command;
		public override string Description => "Removes all brambles across the map (may take a while).";


		////////////////

		public override void Action( CommandCaller caller, string input, string[] args ) {
			int brambleType = ModContent.TileType<CursedBrambleTile>();

			Parallel.For( 0, Main.maxTilesX, ( tileX ) => {
				//Parallel.For( 0, Main.maxTilesY, ( tileY ) => {
				for( int tileY=0; tileY<Main.maxTilesY; tileY++ ) {
					Tile tile = Main.tile[tileX, tileY];
					if( tile == null || !tile.active() || tile.type != brambleType ) {
						return;
					}

					lock( BrambleGoneCommand.MyLock ) {
						TileHelpers.KillTile( tileX, tileY, false, false );
					}
				}
			} );
		}
	}
}
