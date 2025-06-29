using System;

namespace UnityEngine {
	public struct AudioConfiguration {
		public int numRealVoices {
			get { return 1024; }
			set { }
		}

		public int numVirtualVoices {
			get { return 1024; }
			set { }
		}

		public AudioSpeakerMode speakerMode;
		public int dspBufferSize;
		public int sampleRate;
	}
}