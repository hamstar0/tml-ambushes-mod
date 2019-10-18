using HamstarHelpers.Helpers.Debug;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace Ambushes {
	abstract partial class Ambush {
		public abstract bool OnActivate( int openTileX, int openTileY );


		public abstract void OnDeactivate();


		public abstract bool Run();


		public abstract void EditSpawnPool( IDictionary<int, float> pool, NPCSpawnInfo spawnInfo );


		public abstract void EditSpawnData( Player player, ref int spawnRate, ref int maxSpawns );
	}
}
