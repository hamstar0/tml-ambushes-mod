﻿using HamstarHelpers.Classes.Tiles.TilePattern;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Tiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Ambushes {
	abstract partial class Ambush {
		private ISet<int> ClaimedNpcWhos = new HashSet<int>();


		////////////////

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

			if( !isDone ) {
				if( this.PlaysMusic && this.TriggeringPlayer == Main.myPlayer ) {
					ModContent.GetInstance<AmbushesWorld>()?
						.AmbushMngr
						.PlayMusic( this );
				}

				this.FlushDeadNPCs();
			}
		}

		internal void InternalUpdateNPCForAmbush( NPC npc ) {
			if( !this.ClaimedNpcWhos.Contains(npc.whoAmI) && this.AttemptClaimNPC(npc) ) {
				this.OnClaimNPC( npc );
			}
			if( this.ClaimedNpcWhos.Contains(npc.whoAmI) ) {
				this.UpdateNPCForAmbush( npc );
			}
		}


		////////////////

		public bool Trigger( Player player ) {
			(int x, int y)? point = TileFinderHelpers.GetNearestTile( this.TileX, this.TileY, TilePattern.NonSolid, 12 );
			if( !point.HasValue ) {
				LogHelpers.Warn( "No empty air for ambush to trigger." );
				return false;
			}

			this.TriggeringPlayer = player.whoAmI;
			
			return this.OnActivate( point.Value.x, point.Value.y );
		}


		////////////////

		public bool ArePlayersNearby( int tileRadius ) {
			int minDistSqr = tileRadius << 4;
			minDistSqr *= minDistSqr;

			int plrMax = Main.player.Length - 1;
			for( int i = 0; i < plrMax; i++ ) {
				Player plr = Main.player[i];
				if( plr == null || !plr.active || plr.dead ) {
					continue;
				}

				if( Vector2.DistanceSquared( plr.Center, this.WorldPosition ) < minDistSqr ) {
					return true;
				}
			}

			return false;
		}
	}
}
