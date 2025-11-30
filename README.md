# 🏹 Archer-Clone (Archero 2 Style 3D Roguelike)
**"생각은 짧게, 구현은 빠르게"**

3주 만에 완성하는 '궁수의 전설 2' 스타일의 3D 액션 로그라이크 슈터 프로젝트입니다. Unity 2022.3 LTS를 기반으로 하며, SOLID 원칙과 **검증된 디자인 패턴(Strategy, Factory, HFSM)**을 적용하여 확장성 높은 스킬 시스템과 견고한 게임 아키텍처를 구축했습니다.

---

# 🎮 1. Game Overview (게임 개요)

**장르:** 3D 액션 로그라이크 (Action Roguelike)

**핵심 루프:**  
`[방 입장] → [문 닫힘 & 웨이브 전투] → [레벨업 & 스킬 선택] → [문 열림] → [보스전]`

### 주요 특징

- **Hit & Run:** 이동 시 공격 중지, 정지 시 자동 공격 (모바일 최적화 조작)
- **Infinite Synergy:** 멀티샷 × 도탄 × 화염 등 전략적 스킬 조합
- **Stage Based:** StageManager 중심의 방/웨이브/문 시스템
- **Permanent Growth:** StatDataSO & LevelDataSO 기반 성장 구조

---

# 🛠 2. Tech Stack & Architecture (기술 스택 및 아키텍처)

## 2.1 Core Design Patterns

### 🔹 Strategy Pattern (유연한 스킬 시스템)

**목적:**  
if-else 없이 투사체/패시브 스킬 로직을 동적으로 조합

**구현 요소:**  
- `IProjectileStrategy` — OnShoot, OnHit 행동 정의  
- `IPassiveStrategy` — OnEquip, OnUpdate 버프 기능 정의  
- 모든 전략은 `03_Skill/Active | Passive` 폴더에서 모듈식 관리

---

### 🔹 Factory Method Pattern (데이터 → 로직 변환)

**목적:**  
ScriptableObject(SO)의 데이터를 기반으로 실제 전략 객체 생성

**구현:**  
`SkillDataSO.CreateStrategy()` 호출 → 설정값 주입된 `new Strategy(...)` 반환

---

### 🔹 Hierarchical Finite State Machine (HFSM)

**목적:**  
플레이어/적 상태를 명확히 분리하여 관리

**구현:**  
- StateMachine 기반  
- Dictionary + `Func<bool>` 조건 전이  
- Idle → Move → Attack → Dead 등 계층 구조 적용

---

### 🔹 Data-Driven Design (데이터 주도 설계)

**목적:**  
코드 수정 없이 밸런싱 변경 가능

**데이터 구성:**  
- `StatDataSO`: HP, ATK, SPD 등 기본 능력치  
- `LevelDataSO`: 경험치 요구량 테이블  
- PlayerStat/LivingEntity는 SO를 통한 값 초기화

---

## 2.2 Implementation Details (상세 구현)

### Physics & Movement
- **Player:** CharacterController (부드러운 이동, 물리 떨림 없음)
- **Enemy:** NavMeshAgent (길찾기 지원)
- **Projectile:** Rigidbody + SphereCollider(Trigger) / FixedUpdate 이동

### Interfaces
모든 인터페이스를 `PublicInterface.cs`에 통합 → 순환 의존성 제거

### Managers
- **StageManager:** 웨이브 관리  
- **GameManager:** 게임 흐름 제어  
- **PoolManager:** 투사체 및 이펙트 최적화  

---

# 📂 3. Project Structure (최종 아키텍처 v3.6)

```
Assets/
├── 01_Scenes/
│   ├── 0_Final/            # 최종 빌드에 포함될 완성된 씬 폴더
│   ├── Lobby_Temp.unity    # 로비 기능 테스트용 임시 씬
│   ├── Stage_Temp.unity    # 인게임 전투 테스트용 임시 씬
│   └── (Dev_Folders)/      # 팀원별 개인 샌드박스 폴더 (CBY, JJH...)
│
├── 02_Scripts/
│   ├── 00_Public/
│   │   ├── Core/               # [Foundation] 프로젝트의 핵심 뼈대 (모든 곳에서 참조 가능)
│   │   │   ├── Defines.cs      # [Const] 태그, 레이어, 씬 이름, Enum 상수 정의
│   │   │   ├── LivingEntity.cs # [Base] HP를 가지는 모든 생명체의 최상위 부모 클래스
│   │   │   ├── ObjectPool.cs   # [Logic] 제네릭 기반 오브젝트 풀링 알고리즘
│   │   │   ├── StateMachine.cs # [Logic] HFSM 상태 머신 베이스 클래스
│   │   │   └── Utils.cs        # [Tool] 디버그 로그 및 헬퍼 함수 모음
│   │   │
│   │   ├── Managers/           # [Singleton] 게임 전반을 관리하는 매니저들
│   │   │   ├── GameManager.cs  # 게임 상태(전투, 일시정지, 종료) 관리
│   │   │   ├── PoolManager.cs  # ObjectPool을 실제로 생성하고 관리하는 매니저
│   │   │   ├── StageManager.cs  # 방 입장, 웨이브 스폰, 문 개방 로직 담당
│   │   │   └── Managers.cs     # 다른 매니저들에 접근하기 위한 진입점 (EntryPoint)
│   │   │
│   │   ├── SO/                 # [ScriptableObject] 데이터 클래스 정의 (설계도)
│   │   │   ├── EventChannelSO/ # 옵저버 패턴용 이벤트 채널 정의
│   │   │   ├── SkillDataSO/    # 스킬 속성 정의 및 전략 객체 생성(Factory) 로직
│   │   │   ├── StatDataSO/     # 캐릭터/적의 기본 능력치 데이터 정의
│   │   │   ├── LevelDataSO/    # 레벨별 요구 경험치 테이블 정의
│   │   │   └── ItemDataSO/     # 아이템 드랍 및 속성 정의
│   │   │
│   │   └── PublicInterface.cs  # [Interface] IDamageable, IProjectileStrategy 등 인터페이스 통합 관리
│   │
│   ├── 01_Player/              # 플레이어 관련 로직
│   │   ├── PlayerController.cs # 입력 처리(Input System) 및 이동(CharacterController)
│   │   ├── PlayerStat.cs       # StatDataSO를 참조하여 런타임 능력치 관리
│   │   ├── PlayerLevel.cs      # LevelDataSO를 참조하여 경험치 및 레벨업 관리
│   │   └── State/              # 플레이어 상태 클래스 (Idle, Move, StopState)
│   │
│   ├── 02_Enemy/               # 적 AI 관련 로직
│   │   ├── EnemyBase.cs        # NavMeshAgent 기반 적 공통 로직 (LivingEntity 상속)
│   │   ├── Enemys/             # 개별 몬스터 구현 (근거리, 원거리, 보스)
│   │   └── State/              # 적 FSM 상태 클래스
│   │
│   ├── 03_Skill/               # [Strategy Pattern] 스킬 시스템 코어
│   │   ├── Active/             # 능동 스킬 (투사체 등)
│   │   │   ├── Projectile.cs           # 전략을 수행하는 투사체 컨텍스트
│   │   │   └── Strategies/             # 실제 구현체 (Ricochet, Multishot, Fire...)
│   │   │
│   │   └── Passive/            # 패시브 스킬 (버프, 소환 등)
│   │       └── Strategies/             # 실제 구현체 (StatBoost, RotatingShield...)
│   │
│   └── 04_UI/                  # UI 스크립트
│       ├── LobbyScene/         # 로비 전용 UI
│       └── StageScene/         # 인게임 HUD, Pause, 결과창 UI
│
├── 03_Prefabs/                 # 인게임에서 생성되는 실제 오브젝트 및 데이터 에셋
│   ├── 01_SO/                  # [Data] ScriptableObject 실제 데이터 파일 (.asset)
│   │   ├── EventChannel/       # 각종 이벤트 채널 에셋
│   │   ├── SkillData/          # 스킬 설정값 (도탄_Lv1, 멀티샷_Lv1 등)
│   │   ├── StatData/           # 스탯 테이블
│   │   │   ├── Player/         # PlayerStatData.asset (기본 스탯)
│   │   │   └── Enemy/          # EnemyStatData.asset (몬스터별 스탯)
│   │   └── LevelData/          # LevelTable.asset (경험치 테이블)
│   │
│   ├── 02_Managers/            # DDOL 매니저 프리팹
│   │   ├── GameManager.prefab
│   │   └── PoolManager.prefab
│   │
│   ├── 03_Player/              # 플레이어 관련 프리팹
│   │
│   ├── 04_Enemy/               # 적 관련 프리팹
│   │
│   ├── 05_Skill/               # 스킬 관련 오브젝트 프리팹
│   │   ├── Projectiles/        # 화살, 마법구 등 투사체
│   │   └── Effects/            # 피격 이펙트, 폭발 이펙트 등
│   │
│   ├── 06_UI/                  # UI 프리팹(아래 예시)
│   │   ├── HUD_Canvas.prefab   # 조이스틱, HP바, 경험치바
│   │   ├── Popup_Pause.prefab
│   │   ├── Popup_SkillSelect.prefab # 레벨업 시 뜰 스킬 선택창
│   │   └── DamageText.prefab   # 데미지 플로팅 텍스트
│   │
│   └── 07_Environment/         # [New] 맵 구성 요소(아래 예시)
│       ├── Map_Room_01.prefab
│       ├── Prop_Obstacle.prefab
│       └── Door_Gate.prefab
│
├── 04_Materials/               # 머티리얼 및 텍스처
└── 05_Animation/               # 애니메이션 클립 및 컨트롤러
```

---

# 📅 4. Development Roadmap (개발 로드맵)

## 🟢 1주차: Foundation (기반 구축)
- HFSM 프레임워크 구축  
- Player 이동 + 기본 공격  
- PlayerStat / LevelDataSO  
- Input System 조작  
- Enemy NavMesh 추적 + 피격

## 🟡 2주차: System Expansion (시스템 확장)
- SkillDataSO 팩토리 완성  
- Projectile 전략 패턴 (도탄, 멀티샷 등)  
- LevelUp UI + 경험치 시스템  
- StageManager로 문 제어  
- PoolManager로 최적화

## 🔴 3주차: Content & Polish (콘텐츠 및 마감)
- 패시브 스킬 (회전 방패, 자동 터렛)  
- 보스 행동 패턴  
- 스테이지 연출, 데미지 텍스트  
- 카메라 쉐이크, 사운드, 쉐이더

---

# ⚠️ Conventions & Rules (개발 규칙)

팀원 간의 코드 일관성과 원활한 협업을 위해 아래 규칙을 **반드시 준수**해 주세요.

## 1. Coding Standards (코딩 컨벤션)

### Naming & Syntax (명명 규칙)

- Private 멤버 변수: m + PascalCase (대문자로 시작)
  private float mCurrentHp;           // Good
  private float currentHp;            // Bad
  private float _currentHp;           // Bad

- Interface: 이름 앞에 I 접두사 필수
  IProjectileStrategy, IDamageable

- Enum: 이름 앞에 E 접두사 필수
  EEnemyState, ESkillType

### Safety & Optimization (안전성 및 최적화)

- String 리터럴 사용 금지: Tag, Scene 등은 무조건 Defines.cs의 const 사용
  CompareTag(Define.Tag_Player)      // Good
  CompareTag("Player")               // Bad

- Animator String 금지: AnimHash 클래스의 int 값만 사용
  animator.SetTrigger(AnimHash.Attack);    // Good
  animator.SetTrigger("Attack");           // Bad

- LayerMask는 Layers 헬퍼 클래스 사용
  int mask = Layers.GetMask(ELayerName.Enemy);

- 이벤트 통신
  → 직접 참조가 명확하면 C# Action/Event
  → 결합도 낮추거나 1:N 필요하면 EventChannelSO 권장

### Unity Basic Rules (기본 규칙)

- new MonoBehaviour() 절대 금지 → AddComponent 또는 Instantiate만 사용
- 모든 인터페이스는 00_Public/PublicInterface.cs에 통합 관리
- 밸런스 수치 하드코딩 금지 → StatDataSO 등 ScriptableObject 참조
- Physics 컴포넌트 표준
  Player      → CharacterController
  Enemy       → NavMeshAgent
  Projectile  → Rigidbody (IsTrigger On, Use Gravity Off)

## 2. Git & GitHub Workflow (협업 규칙)

충돌 최소화를 위해 "순차적 머지(Sequential Merge)" 방식 사용

### Branch Strategy
- main 브랜치: 직접 커밋 금지 (Protected)
- 작업은 항상 dev → feat/이니셜 브랜치에서만 (예: feat/KYB, feat/JSH)

### Commit & PR Process
1. dev 최신화 (checkout dev → pull)
2. 개인 브랜치 생성 후 작업
3. 작업 완료 또는 정해진 시간 → dev로 PR
4. PR 올린 후 즉시 작업 멈춤 (추가 커밋 금지!!)
5. 머지 완료 후 → 기존 개인 브랜치 반드시 삭제
6. 다시 dev pull → 새 브랜치 생성 → 반복

### Commit Message 규칙
feat/이니셜 : 핵심 변경 사항 요약
- 상세 내용 1
- 상세 내용 2

예시:
feat/KYB : 플레이어 이동 및 점프 구현
- CharacterController 이동 로직 추가
- 점프 및 이중점프 처리
- 착지 사운드 재생

### 정기 PR 시간표 (Traffic Control)

| 순서 | 팀원   | PR 마감 시간 | 비고                     |
|------|--------|--------------|--------------------------|
| 1    | 팀원 A | 18:30        | 가장 먼저 PR 후 머지 대기 |
| 2    | 팀원 B | 19:00        | A 머지 완료 후 PR        |
| 3    | 팀원 C | 19:30        | B 머지 완료 후 PR        |

### 긴급 상황 (Merge Conflict)
- 충돌 발생 시 팀장이 즉시 전체 공지
- 뒷 순번은 공지 올 때까지 PR 대기

### 수시 PR
- 다른 팀원과 연동되는 핵심 기능 완성 시 → 시간 무관 즉시 PR + 팀 채팅 공지

---

# 👨‍💻 Contributors (Oz_Team24)
**Role:** Unity Client Developer  
**Engine:** Unity 2022.3 LTS
