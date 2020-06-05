using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;

public class Deadlock : MonoBehaviour {

    public KMBombInfo Bomb;
    public KMAudio Audio;
    public KMSelectable[] Arrows;
    public KMSelectable Reset;
    public KMSelectable[] Cubes;
    public Material[] Colors; //Red blue orange black white yellow green

    /*
    movements

    white = -1
    blue = +1
    yellow = -3
    orange = +3
    green = -9
    red = +9


    */
    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;
    private List<int> ColorTracker = new List<int>{};
    private List<int> Movements = new List<int>{9,3,1,0,-1,-3,-9,69};
    private List<int> CubeOfPressing = new List<int>{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
    int you = 0;
    int ColorRandomizer = 0;
    int StageNumber = 1;
    int StageNumberForColorTracker = 0;
    int LastMovement = 0;
    bool Didpass = false;
    int SwitchForColor = 0;
    bool Zero = false;
    int ForSwitchMovement = 0;
    int integer = 0;

    void Awake () {
        moduleId = moduleIdCounter++;

        foreach (KMSelectable Arrow in Arrows) {
            KMSelectable pressedArrow = Arrow;
            Arrow.OnInteract += delegate () { ArrowPress(pressedArrow); return false; };
        }

        Reset.OnInteract += delegate () { PressReset(); return false; };

        foreach (KMSelectable Cube in Cubes) {
            KMSelectable pressedCube = Cube;
            Cube.OnInteract += delegate () { CubePress(pressedCube); return false; };
        }
    }

    void Start () {
      Whatthefuck();
      switch (Bomb.GetBatteryCount() % 6) {
        case 0:
        Movements[7] = -9;
        break;
        case 1:
        Movements[7] = -3;
        break;
        case 2:
        Movements[7] = -1;
        break;
        case 3:
        Movements[7] = 1;
        break;
        case 4:
        Movements[7] = 3;
        break;
        case 5:
        Movements[7] = 9;
        break;
      }
      CubeDeterminer();
	}
  void ArrowPress(KMSelectable Arrow) {
    if (Arrow == Arrows[0]) {
      StageNumber -= 1;
      if (StageNumber == 0) {
        StageNumber = 8;
      }
      StartCoroutine(ColorSetter());
    }
    else if (Arrow == Arrows[1]) {
      StageNumber += 1;
      if (StageNumber == 9) {
        StageNumber = 1;
    }
    StartCoroutine(ColorSetter());
  }
}
  void PressReset(){
    StageNumber = 1;
    StartCoroutine(ColorSetter());
  }
  void CubePress(KMSelectable Cube) {
    for(int i = 0; i < 27; i++){
      if (Cube == Cubes[i]) {
        if (CubeOfPressing[i] == 2) {
          GetComponent<KMBombModule>().HandlePass();
        }
        Debug.Log(i);
      }
    }
  }
  void Whatthefuck(){
      for (int i = 0; i < 243; i++) {
      ColorTracker.Add(UnityEngine.Random.Range(0,7));
      }
      for (int i = 0; i < 27; i++) {
        Cubes[i].GetComponent<MeshRenderer>().material = Colors[ColorTracker[i]];
      }
      you = UnityEngine.Random.Range(0,Cubes.Count());
      ColorTracker[you] = 7;
      Cubes[you].GetComponent<MeshRenderer>().material = Colors[7];
    }
    IEnumerator ColorSetter(){
      for (int j = 0; j < 27; j++) {
        switch (StageNumber) {
          case 1:
           Cubes[j].GetComponent<MeshRenderer>().material = Colors[ColorTracker[j]];
          break;
          case 2:
           Cubes[j].GetComponent<MeshRenderer>().material = Colors[ColorTracker[j + 27]];
          break;
          case 3:
           Cubes[j].GetComponent<MeshRenderer>().material = Colors[ColorTracker[j + (27 * 2)]];
          break;
          case 4:
           Cubes[j].GetComponent<MeshRenderer>().material = Colors[ColorTracker[j + (27 * 3)]];
          break;
          case 5:
           Cubes[j].GetComponent<MeshRenderer>().material = Colors[ColorTracker[j + (27 * 4)]];
          break;
          case 6:
           Cubes[j].GetComponent<MeshRenderer>().material = Colors[ColorTracker[j + (27 * 5)]];
          break;
          case 7:
           Cubes[j].GetComponent<MeshRenderer>().material = Colors[ColorTracker[j + (27 * 6)]];
          break;
          case 8:
           Cubes[j].GetComponent<MeshRenderer>().material = Colors[ColorTracker[j + (27 * 7)]];
          break;
        }
      }
      yield return null;
    }

    void CubeDeterminer(){
        ForSwitchMovement = Movements[ColorTracker[you + (27 * integer) % 243]];
        something:
        switch (ForSwitchMovement) {
          case 0:
          ForSwitchMovement = LastMovement;
          goto something;
          break;
          case -9:
          Debug.Log(you);
          LastMovement = -9;
          you += Movements[ColorTracker[you]];
          if (you < 0) {
            you += 27;
          }
          if (CubeOfPressing[you] == 1) {
            you += Movements[ColorTracker[you]];
            if (you < 0) {
              you += 27;
            }
            if (CubeOfPressing[you] == 1 && you > 8) {
              CubeOfPressing[you - 9] += 1;
              return;
            }
            else if (CubeOfPressing[you] == 1) {
              CubeOfPressing[you + 18] += 1;
              return;
            }
            else {
              CubeOfPressing[you] = 1;
            }
          }
          else {
            CubeOfPressing[you] = 1;
          }
          break;
          case -3:
          Debug.Log(you);
          LastMovement = -3;
          if (you == 0 || you == 1 || you == 2 || you == 9 || you == 10 || you == 11 || you == 18 || you == 19 || you == 20) {
            you += 6;
          }
          else {
            you += Movements[ColorTracker[you]];
          }
          if (CubeOfPressing[you] == 1) {
            if (you == 0 || you == 1 || you == 2 || you == 9 || you == 10 || you == 11 || you == 18 || you == 19 || you == 20) {
              you += 6;
            }
            else {
              you += Movements[ColorTracker[you]];
            }
            if (CubeOfPressing[you] == 1 && (you == 0 || you == 1 || you == 2 || you == 9 || you == 10 || you == 11 || you == 18 || you == 19 || you == 20)) {
              CubeOfPressing[you + 6] += 1;
              return;
            }
            else if (CubeOfPressing[you] == 1) {
              CubeOfPressing[you - 3] += 1;
              return;
            }
            else {
              CubeOfPressing[you] = 1;
            }
          }
          else {
            CubeOfPressing[you] = 1;
          }
          break;
          case -1:
          Debug.Log(you);
          LastMovement = -1;
          if (you == 0 || you == 3 || you == 6 || you == 9 || you == 12 || you == 15 || you == 18 || you == 21 || you == 24) {
            you += 2;
          }
          else {
            you += Movements[ColorTracker[you]];
          }
          if (CubeOfPressing[you] == 1) {
            if (you == 0 || you == 3 || you == 6 || you == 9 || you == 12 || you == 15 || you == 18 || you == 21 || you == 24) {
              you += 2;
            }
            else {
              you += Movements[ColorTracker[you]];
            }
            if (CubeOfPressing[you] == 1 && (you == 0 || you == 3 || you == 6 || you == 9 || you == 12 || you == 15 || you == 18 || you == 21 || you == 24)) {
              CubeOfPressing[you + 2] += 1;
              return;
            }
            else if (CubeOfPressing[you] == 1) {
              CubeOfPressing[you - 1] += 1;
              return;
            }
            else {
              CubeOfPressing[you] = 1;
            }
          }
          else {
            CubeOfPressing[you] = 1;
          }
          break;
          case 1:
          Debug.Log(you);
          LastMovement = 1;
          if (you == 2 || you == 5 || you == 8 || you == 11 || you == 14 || you == 17 || you == 20 || you == 23 || you == 26) {
            you -= 2;
          }
          else {
            you += Movements[ColorTracker[you]];
          }
          if (CubeOfPressing[you] == 1) {
            if (you == 2 || you == 5 || you == 8 || you == 11 || you == 14 || you == 17 || you == 20 || you == 23 || you == 26) {
              you -= 2;
            }
            else {
              you += Movements[ColorTracker[you]];
            }
            if (CubeOfPressing[you] == 1 && (you == 2 || you == 5 || you == 8 || you == 11 || you == 14 || you == 17 || you == 20 || you == 23 || you == 26)) {
              CubeOfPressing[you - 2] += 1;
              return;
            }
            else if (CubeOfPressing[you] == 1) {
              CubeOfPressing[you + 1] += 1;
              return;
            }
            else {
              CubeOfPressing[you] = 1;
            }
          }
          else {
            CubeOfPressing[you] = 1;
          }
          break;
          case 3:
          Debug.Log(you);
          LastMovement = 3;
          if (you == 6 || you == 7 || you == 8 || you == 15 || you == 16 || you == 17 || you == 24 || you == 25 || you == 26) {
            you -= 6;
          }
          else {
            you += Movements[ColorTracker[you]];
          }
          if (CubeOfPressing[you] == 1) {
            if (you == 6 || you == 7 || you == 8 || you == 15 || you == 16 || you == 17 || you == 24 || you == 25 || you == 26) {
              you += 6;
            }
            else {
              you += Movements[ColorTracker[you]];
            }
            if (CubeOfPressing[you] == 1 && (you == 6 || you == 7 || you == 8 || you == 15 || you == 16 || you == 17 || you == 24 || you == 25 || you == 26)) {
              CubeOfPressing[you - 6] += 1;
              return;
            }
            else if (CubeOfPressing[you] == 1) {
              CubeOfPressing[you + 3] += 1;
              return;
            }
            else {
              CubeOfPressing[you] = 1;
            }
          }
          else {
            CubeOfPressing[you] = 1;
          }
          break;
          case 9:
          Debug.Log(you);
          LastMovement = 9;
          you += Movements[ColorTracker[you]];
          if (you > 26) {
            you -= 27;
          }
          if (CubeOfPressing[you] == 1) {
            you += Movements[ColorTracker[you]];
            if (you < 0) {
              you += 27;
            }
            if (CubeOfPressing[you] == 1 && you < 18) {
              CubeOfPressing[you + 9] += 1;
              return;
            }
            else if (CubeOfPressing[you] == 1) {
              CubeOfPressing[you - 18] += 1;
              return;
            }
            else {
              CubeOfPressing[you] = 1;
            }
          }
          else {
            CubeOfPressing[you] = 1;
          }
          break;
        }
      integer += 1;
      CubeDeterminer();
    }
  }
