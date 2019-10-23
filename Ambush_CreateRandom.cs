using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Helpers.TModLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace Ambushes {
	abstract partial class Ambush {
		protected static List<Ambush> StaticInstances;
		protected static float TotalWeight = 0f;



		////////////////

		internal static void OnModsLoad() {
			IEnumerable<Type> ambushTypes = ReflectionHelpers.GetAllAvailableSubTypesFromMods( typeof(Ambush) )
				.Where( t => !t.IsAbstract );

			Ambush.StaticInstances = ambushTypes.SafeSelect(
				ambushType => (Ambush)Activator.CreateInstance(ambushType, true)
			).SafeOrderBy( a => {
				Ambush.TotalWeight += a.SpawnWeight;
				return a.SpawnWeight;
			} ).ToList();
		}

		internal static void OnModsUnload() {
			Ambush.StaticInstances = null;
		}


		////////////////

		public static Ambush CreateRandomType( int tileX, int tileY ) {
			float rand = TmlHelpers.SafelyGetRand().NextFloat() * (float)Ambush.TotalWeight;
			Ambush randAmbush = null;

			float counted = 0f;
			foreach( Ambush ambush in Ambush.StaticInstances ) {
				if( rand >= counted && rand < ( ambush.SpawnWeight + counted ) ) {
					randAmbush = ambush;
					break;
				}
				counted += ambush.SpawnWeight;
			}
			
			return randAmbush?.Clone( tileX, tileY );
		}
	}
}
