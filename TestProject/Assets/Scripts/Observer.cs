using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EVENTNAME { MUSIC, EFFECT }

public interface Observer {

	void onNotify(float value, EVENTNAME eventName);
}
