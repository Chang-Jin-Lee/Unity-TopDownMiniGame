# UnityGame - 탑다운 슈팅 미니게임

<div align="center">
<table>
  <tr>
    <td><img src="https://github.com/user-attachments/assets/6a00543f-7f6d-4f4b-88a8-e59366c6c495" alt="Midori_Title" width="300"/></td>
    <td><img src="https://github.com/user-attachments/assets/caffa465-686b-402c-978c-2548d6223192" alt="Momoi_Title" width="300"/></td>
  </tr>
</table>
<table>
  <tr>
    <td><img src="https://github.com/user-attachments/assets/3f998a08-b724-4375-975d-52fe7e5a854e" alt="Midori_Title" width="300"/></td>
    <td><img src="https://github.com/user-attachments/assets/7e34aa68-3152-4c93-80b5-000cce0a2481" alt="Momoi_Title" width="300"/></td>
  </tr>
</table>
</div>

Unity 기반의 탑다운 슈팅 게임입니다.

* 프로젝트 목적 : Unity Sprite, Animator, AnimationController, Generic 타입의 Singleton 객체, UI Delegate의 학습을 위해

* 프로젝트를 통해 알게 된 점 : 제네릭 타입의 사용 및 싱글톤 객체 관리에 대해 알게 되었습니다. Sprite Animation을 만들고 코드로 제어하는 방법, Inspector Window에 보이는 Property의 이름을 커스텀으로 바꾸는 방법을 알게 되었습니다.

* 프로젝트 기간 : 2025.04.23 \~ 2025.05.01 (7일)

* 프로젝트 인원 : 1인

## 🔍 플레이 장면

<div align="center">
<table>
  <tr>
    <td><img src="https://github.com/user-attachments/assets/7611606b-5e18-467d-93f5-1a1e3c213fe4" alt="Midori_Title" width="400"/></td>
  </tr>
</table>
</div>

---

## 🎮 주요 기능

### ✅ 캐릭터 선택 및 커서 이미지 변경

* **Midori / Momoi 캐릭터 선택 가능**
* 각 캐릭터에 따라 Live2D 애니메이션 및 마우스 커서 이미지 변경
* `MainCanvas`의 `ShowLive2D`, `UpdateCursor` 함수로 제어

```csharp
void CharacterSelected(eCharacterState newState)
{
    gameState.SetCharacterState(newState);
    ShowLive2D(newState, eLive2DState.Start);
    UpdateCursor(); // 커서 동적 변경
}
```

---

### ✅ Live2D 애니메이션 구조

* 각 캐릭터 상태 범위에 따라 `live2DAnimations` GameObject 배열에 등록
* 인스펙터에서 `MainCanvasEditor`를 통해 커스터마이징 가능
* 애니메이션은 실시간 전환되며 하나만 활성화

```csharp
void ShowLive2D(eCharacterState characterState, eLive2DState live2DState)
{
    foreach (var group in live2DGroups)
        foreach (var anim in group.live2DAnimations)
            if (anim != null) anim.SetActive(false);

    live2DGroups[(int)characterState].live2DAnimations[(int)live2DState].SetActive(true);
}
```

---

### ✅ 플레이 UI 및 옵션 메뉴

* `TimeManager`, `GameState`를 통해 남은 시간, 적 수, 킬 수를 UI에 실시간 표시
* `OptionMenu`는 `Time.timeScale = 0`을 통해 게임 일시 정지
* 배경 애니메이션은 `Animator`의 `Update Mode = Unscaled Time` 설정으로 계속 재생

<div align="center">
<table>
  <tr>
    <td><img src="https://github.com/user-attachments/assets/e3a68b38-2928-4d3e-aec0-ae08b2b25b71" alt="Midori_Title" width="400"/></td>
  </tr>
</table>
</div>

---

## 💡 새로 알게 된 점 정리

### 🔧 커스텀 에디터

* `[CustomPropertyDrawer(typeof(ClassName))]`를 통해 클래스 프로퍼티 배열의 인덱스를 커스텀 이름으로 직렬화
* `[System.Serializable]`, `PropertyDrawer`를 통해 배열 내부 요소를 enum의 이름과 함께 출력
* 자동으로 높이를 계산해줄 수 있도록 GetPropertyHeight() 오버라이드

```csharp
for (int i = 0; i < statesProp.arraySize; i++) 
{
    string stateName = ((eLive2DState)i).ToString();
    EditorGUI.PropertyField(..., new GUIContent(stateName));
}
```

> 예시 

```csharp
public enum eCharacterState
{
    Midori,
    Momoi,
    Max,
}
public enum eLive2DState
{
    Start,
    Idle,
    Maid_Start,
    Maid_Idle,
    Max
}
```


<div align="center">
  <table>
    <tr>
      <td align="center" style="padding: 10px;">
        <div style="margin-bottom: 8px; font-weight: bold; font-size: 16px;"> CustomEditor 적용 전</div>
        <img src="https://github.com/user-attachments/assets/dfb0ac5c-3ad9-41a7-8051-1db4b9ea28b9" alt="Before" width="400"/>
      </td>
      <td align="center" style="padding: 10px;">
        <div style="margin-bottom: 8px; font-weight: bold; font-size: 16px;"> CustomEditor 적용 후</div>
        <img src="https://github.com/user-attachments/assets/b103e3ad-af71-446a-9a90-00b58451166b" alt="After" width="400"/>
      </td>
    </tr>
  </table>
</div>

### 🎯 인터페이스 구조

* `IPlayerAbility`, `IWeaponAbility`, `IBulletAbility` 등으로 역할 분리
* 필요한 Ability를 인터페이스로 상속받아 사용
* 컴포넌트 탐색을 위해 `ComponentHelper.FindInterface<T>()` 사용

```csharp
public static T FindInterface<T>(GameObject target) where T : class
{
    var components = target.GetComponents<MonoBehaviour>();
    foreach (var comp in components)
        if (comp is T match) return match;
    return null;
}
```

### 🧠 게임 상태 관리

* 전역 싱글톤 `public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour`를 통해 점수, 시간, 캐릭터, 씬 상태를 통합 관리
* `Action<T>` 델리게이트를 활용해 UI는 자동 업데이트

<div align="center">
<table>
  <tr>
    <td><img src="https://github.com/user-attachments/assets/12b7f983-cbaf-4f4a-a6ce-5efa08f2599f" alt="Midori_Title" width="400"/></td>
  </tr>
</table>
</div>
