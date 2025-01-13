using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace TrungNV {
	public class TrufnCache<T> {
		private static Dictionary<Collider2D, T> characters = new();

		public static T GetCol2D(Collider2D col) {
			if (!characters.ContainsKey(col)) {
				characters.Add(col, col.GetComponent<T>());
			}

			return characters[col];
		}
	}

	public class TrufnCache {
		private static Dictionary<float, WaitForSeconds> WFS = new();

		public static WaitForSeconds GetWFS(float key)
		{
			if (!WFS.ContainsKey(key)) {
				WFS[key] = new WaitForSeconds(key);
			}

			return WFS[key];
		}
	}
}