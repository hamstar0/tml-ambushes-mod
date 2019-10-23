using HamstarHelpers.Classes.Tiles.TilePattern;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Tiles;
using HamstarHelpers.Services.OverlaySounds;
using Microsoft.Xna.Framework;
using System;
using Terraria;


namespace Ambushes {
	abstract partial class Ambush {
		public int TileX { get; }
		public int TileY { get; }

		////

		public int TriggeringPlayer { get; private set; } = -1;

		public bool IsEnded { get; internal set; } = false;

		////

		public Vector2 WorldPosition => new Vector2( this.TileX << 4, this.TileY << 4 );



		////////////////

		public Ambush( int tileX, int tileY ) {
			this.TileX = tileX;
			this.TileY = tileY;
		}


		////////////////

		internal void Deactivate() {
			this.IsEnded = true;
			this.OnDeactivate();
		}


		////////////////

		internal void InternalRun( out bool isDone ) {
			isDone = this.RunUntil();
		}

		////

		internal void InternalNPCPreAI( NPC npc ) {
			this.NPCPreAI( npc );
		}


		////////////////

		public bool Trigger( Player player ) {
			(int x, int y)? point = TileFinderHelpers.GetNearestTile( this.TileX, this.TileY, TilePattern.AbsoluteAir, 8 );
			if( !point.HasValue ) {
				LogHelpers.Warn( "No empty air for ambush to trigger." );
				return false;
			}

			this.TriggeringPlayer = player.whoAmI;
			
			if( player.whoAmI == Main.myPlayer && this.PlaysMusic ) {
				OverlaySound sound = OverlaySound.Create( "Sounds/LowAmbushBGM", 60, 463, 0, -1, () => (1f, this.IsEnded) );
				sound.Play();
			}
			
			return this.OnActivate( point.Value.x, point.Value.y );
		}
	}
}
