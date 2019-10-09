using HamstarHelpers.Helpers.DotNET.Extensions;
using System;
using System.Collections.Generic;
using System.Threading;


namespace Ambushes {
	class AmbushManager {
		#region StaticFields
		private static object MyLock = new object();
		#endregion


		#region StaticProperties
		public static bool IsLocked => Monitor.TryEnter( AmbushManager.MyLock );
		#endregion



		#region InstanceFields
		private IDictionary<int, IDictionary<int, Ambush>> Ambushes
			= new Dictionary<int, IDictionary<int, Ambush>>();
		#endregion


		#region InstanceProperties
		public int Count {
			get {
				lock( AmbushManager.MyLock ) {
					return this.Ambushes.Count2D();
				}
			}
		}
		#endregion



		#region InstanceMethods
		public AmbushManager() { }


		////////////////

		public IEnumerable<Ambush> GetAllAmbushes() {
			lock( AmbushManager.MyLock ) {
				foreach( IDictionary<int, Ambush> otherTileYs in this.Ambushes.Values ) {
					foreach( Ambush ambush in otherTileYs.Values ) {
						yield return ambush;
					}
				}
			}
		}

		////////////////

		public void AddAmbush( Ambush ambush ) {
			lock( AmbushManager.MyLock ) {
				if( !this.Ambushes.ContainsKey( ambush.TileX ) ) {
					this.Ambushes[ambush.TileX] = new Dictionary<int, Ambush>();
				}

				this.Ambushes[ambush.TileX][ambush.TileY] = ambush;
			}
		}

		////

		public void Clear() {
			this.Ambushes.Clear();
		}
		#endregion
	}
}
