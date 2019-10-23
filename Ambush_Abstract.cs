﻿using HamstarHelpers.Helpers.Debug;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Ambushes {
	abstract partial class Ambush {
		public abstract float SpawnWeight { get; }



		////////////////

		protected abstract Ambush CloneRandomized( int tileX, int tileY );


		////////////////

		protected abstract bool OnActivate( int clearTileX, int clearTileY );


		protected abstract void OnDeactivate();


		protected abstract bool RunUntil();


		////////////////

		public virtual void EditNPCSpawnPool( IDictionary<int, float> pool, NPCSpawnInfo spawnInfo ) { }


		public virtual void EditNPCSpawnData( Player player, ref int spawnRate, ref int maxSpawns ) { }


		protected virtual void NPCPreAI( NPC npc ) { }
	}
}
