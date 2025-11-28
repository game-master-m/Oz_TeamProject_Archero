🏹 Archer-Clone (Archero 2 Style 3D Roguelike)

"생각은 짧게, 구현은 빠르게"

3주 만에 완성하는 '궁수의 전설 2' 스타일의 3D 액션 로그라이크 슈터 프로젝트입니다.
검증된 디자인 패턴(Strategy, Factory, Observer)을 적용하여 유연한 스킬 시스템과 최적화된 아키텍처를 구현하는 데 집중했습니다.

🎮 Game Overview (게임 개요)

장르: 3D 액션 로그라이크 (Action Roguelike)
핵심 루프: 입장 → 전투 (이동 & 자동 공격) → 레벨업 & 스킬 선택(랜덤) → 보스전 → 보상 및 성장
특징:
Hit & Run: 이동 시 공격 중지, 정지 시 자동 공격하는 직관적인 조작감.
Random Ability (Roguelike): 매 판 달라지는 스킬 조합 (멀티샷, 화염, 도탄 등).
Permanent Progression: 영구적인 장비 강화 및 재능 시스템.

🛠 Tech Stack & Architecture (기술 스택 및 아키텍처)

본 프로젝트는 유지보수성과 확장성을 고려하여 **객체지향 설계 원칙(SOLID)**을 준수하며, 주요 로직을 시각화된 패턴으로 설계했습니다.


1. Core Design Patterns Visualization

🔹 Strategy Pattern (스킬 시스템)

If-else 분기 없이 투사체의 속성(화염, 빙결, 도탄 등)을 동적으로 조합합니다. ProjectileContext가 여러 개의 전략(Strategy)을 리스트로 관리하며 순차 실행합니다.


🔹 Finite State Machine (플레이어/AI 상태 관리)

복잡한 상태 전이를 클래스로 분리하여 관리합니다.


🔹 Observer Pattern (UI 및 이벤트)

EventChannel을 통해 로직과 UI 간의 결합도를 낮췄습니다.


2. Implementation Details

🔹 유연한 스킬 시스템

기존의 하드코딩된 스킬 구현 방식을 탈피하여, IProjectileStrategy 인터페이스를 통해 스킬을 모듈화했습니다.

구조: 발사/충돌 시점에 각 전략을 순차적으로 실행하여 [멀티샷] + [화염] + [도탄] 등의 무한한 조합이 가능합니다.


🔹 정교한 3D 이동 시스템

Player: Rigidbody 물리 충돌의 불안정함을 제거하기 위해 **CharacterController**를 사용하여 정밀한 이동과 판정을 구현했습니다.

Enemy: NavMeshAgent를 활용하여 장애물을 회피하며 플레이어를 추적하는 AI를 구현했습니다.


📅 Development Roadmap (개발 로드맵)

총 3주간의 스프린트를 통해 단계적으로 기능을 확장했습니다.

🟢 1주차: Foundation (기반 구축)

FSM 프레임워크: IState 인터페이스 및 StateMachine 구현

Player Controller: 조이스틱 입력, CharacterController 이동

Enemy AI: NavMesh 기반 추적 및 기본 공격 로직

UI System: HP바, 데미지 폰트 등 기초 HUD 연결

🟡 2주차: System Expansion (시스템 확장)

Skill Architecture: SkillDataSO 팩토리 및 Strategy 클래스 구현

Skill Selection UI: 레벨업 시 랜덤 스킬 3종 선택 및 인벤토리 적용

Object Pooling: 투사체 및 이펙트 매니저 최적화

🔴 3주차: Content & Polish (콘텐츠 및 마감)

Map Generation: 스테이지 및 문(Door) 시스템, 맵 이동 로직

Boss Battle: 보스 패턴 구현 및 클리어 연출

Optimization: 모바일 타겟 프레임 방어 및 발열 테스트


📂 Project Structure

```csharp
Assets/
├── 01_Scene/
│   ├── MainMenu.unity
│   └── GameStage_01.unity
│
├── 02_Scripts/
│   ├── Core/
│   │   ├── StateMachine.cs
│   │   ├── IState.cs
│   │   ├── Singleton.cs
│   │   └── ObjectPool.cs
│   │
│   ├── Managers/
│   │   ├── GameManager.cs
│   │   ├── SoundManager.cs
│   │   ├── PoolManager.cs
│   │   └── UIManager.cs
│   │
│   ├── Player/
│   │   ├── PlayerController.cs
│   │   ├── PlayerStat.cs
│   │   └── States/
│   │       ├── PlayerIdleState.cs
│   │       ├── PlayerMoveState.cs
│   │       └── PlayerAttackState.cs
│   │
│   ├── Enemy/
│   │   ├── EnemyBase.cs
│   │   ├── EnemyMelee.cs
│   │   ├── EnemyRange.cs
│   │   └── BossEnemy.cs
│   │
│   ├── Skills/ (Strategy Pattern)
│   │   ├── Projectile.cs
│   │   ├── ProjectileContext.cs
│   │   ├── IProjectileStrategy.cs
│   │   └── Strategies/
│   │       ├── FireStrategy.cs
│   │       ├── IceStrategy.cs
│   │       ├── MultiShotStrategy.cs
│   │       ├── RicochetStrategy.cs
│   │       ├── PiercingStrategy.cs
│   │       └── HomingStrategy.cs
│   │
│   ├── Data/ (ScriptableObjects)
│   │   ├── SkillDataSO.cs
│   │   ├── CharacterStatSO.cs
│   │   └── EventChannelSO.cs
│   │
│   └── UI/
│       ├── JoystickController.cs
│       ├── HUDManager.cs
│       ├── DamageText.cs
│       └── SkillSelectUI.cs
│
├── 03_Prefabs/
│   ├── Characters/
│   ├── Projectiles/
│   └── UI/
│
├── 04_Materials/
│   ├── Shaders/
│   └── Textures/
│
├── 05_Animation/
│   ├── Player/
│   └── Enemy/
│
└── 06_AssetPack/
```

[Created by Oz_Team24]
