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
- **Room Based:** RoomManager 중심의 방/웨이브/문 시스템
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
- **RoomManager:** 웨이브 관리  
- **GameManager:** 게임 흐름 제어  
- **PoolManager:** 투사체 및 이펙트 최적화  

---

# 📂 3. Project Structure (최종 아키텍처 v3.6)

```
Assets/
├── 01_Scenes/
│   ├── 0_Final/            # 최종 빌드용 씬
│   ├── Lobby_Temp.unity    # 로비 테스트 씬
│   ├── Stage_Temp.unity    # 인게임 테스트 씬
│   └── (Dev_Folders)/      # 팀원별 개인 작업 폴더
│
├── 02_Scripts/
│   ├── LivingEntity.cs     # [Core] 생명체 베이스 (HP, 사망 처리)
│   │
│   ├── 00_Public/
│   │   ├── Managers/
│   │   │   ├── GameManager.cs
│   │   │   ├── PoolManager.cs  # [Pooling] 오브젝트 풀링
│   │   │   ├── RoomManager.cs  # [Loop] 방/웨이브/문 관리
│   │   │   └── Managers.cs     # 싱글톤 접근자
│   │   │
│   │   ├── SO/
│   │   │   ├── EventChannelSO/ # [Observer]
│   │   │   ├── SkillDataSO/    # [Factory] 스킬 데이터
│   │   │   ├── StatDataSO/     # [Data] 스탯 초기값 (Player/Enemy)
│   │   │   ├── LevelDataSO/    # [Data] 경험치 테이블
│   │   │   └── ItemDataSO/
│   │   │
│   │   ├── PublicEnums.cs      # [Enum] 열거형 통합
│   │   ├── PublicInterface.cs  # [Interface] IDamageable, IStrategy 통합 (★)
│   │   ├── StateMachine.cs     # [HFSM] 상태 머신 코어
│   │   └── Utils.cs
│   │
│   ├── 01_Player/
│   │   ├── PlayerController.cs # [Input] 이동 및 메인 컨트롤러
│   │   ├── PlayerStat.cs       # [Data] StatDataSO 참조 및 런타임 스탯
│   │   ├── PlayerLevel.cs      # [Data] LevelDataSO 참조 및 경험치 로직
│   │   └── State/              # [HFSM] 플레이어 상태 (Idle, Move, Stop)
│   │
│   ├── 02_Enemy/
│   │   ├── EnemyBase.cs        # [AI] NavMesh 및 적 공통
│   │   ├── Enemys/             # 개별 적 구현 (Melee, Range, Boss)
│   │   └── State/              # [HFSM] 적 상태
│   │
│   ├── 03_Skill/               # [Strategy] 스킬 전략 코어
│   │   ├── Active/             # 투사체 및 발사형 스킬
│   │   │   ├── Projectile.cs           # [Context] 투사체 본체
│   │   │   └── Strategies/             # 구현체 (Ricochet, Multishot...)
│   │   │
│   │   └── Passive/            # 버프 및 자동형 스킬
│   │       └── Strategies/             # 구현체 (StatBoost, RotatingShield...)
│   │
│   └── 04_UI/
│
├── 03_Prefabs/
│   ├── 01_SO/              # ScriptableObject 데이터 원본 저장소
│   ├── 02_Managers/
│   ├── 03_Player/
│   ├── 04_Enemy/
│   └── 05_Skill/
│
├── 04_Materials/
└── 05_Animation/
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
- RoomManager로 웨이브/문 제어  
- PoolManager로 최적화

## 🔴 3주차: Content & Polish (콘텐츠 및 마감)
- 패시브 스킬 (회전 방패, 자동 터렛)  
- 보스 행동 패턴  
- 스테이지 연출, 데미지 텍스트  
- 카메라 쉐이크, 사운드, 쉐이더

---

# ⚠️ Conventions & Rules (개발 규칙)

- `new MonoBehaviour()` 절대 사용 금지  
- 인터페이스는 **00_Public/PublicInterface.cs**에만 작성  
- 스탯/밸런스 숫자 하드코딩 금지 → 반드시 SO 참조  
- **Physics 규칙**
  - Player: CharacterController  
  - Enemy: NavMeshAgent  
  - Projectile: Rigidbody(Trigger)

---

# 👨‍💻 Contributors (Oz_Team24)
**Role:** Unity Client Developer  
**Engine:** Unity 2022.3 LTS

---
