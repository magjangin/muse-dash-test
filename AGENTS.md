# Project Instructions

- If a source file grows beyond 600 lines, automatically refactor it into smaller, focused units before adding more substantial logic.
- Keep refactors behavior-preserving unless the user explicitly asks for behavior changes.
- After changes, run `dotnet build` when practical.
- Review `docs/guides/CHECKLIST.md` before finishing a code change and before publishing a release. Every item there records an actual past incident in this repo, not general advice — in particular, verifying the fallback path (no BMS/album → `0-0` fallback → actually enter battle) and checking that the build really landed in the game's `Mods` folder. When a new incident is worth preventing, add an item; when its rationale no longer holds, delete it. (단, 단순 문서 수정이나 게임과 무관한 독립 툴(`SignatureDumper`) 수정 시에는 게임 런타임 관련 항목을 생략하고 해당 범위의 빌드/테스트만 확인합니다.)
- After completing work, always create a git commit.
- Before starting any work, run `git fetch origin` and check `git status -sb` for `ahead/behind` against `origin/main`. This repo is worked on from multiple clones/sessions, and unsynced starts have caused real divergence before (duplicate reimplementation of the same feature, docs reorganized into two different folder schemes on the same day). If behind, pull/rebase before making changes. If diverged (both ahead and behind), stop and surface it instead of guessing which side is authoritative — that call is the user's.
- **Branching & Working Directory**: 게임 모드 코드 변경은 기능 브랜치(feature branch)를 만들어 작업합니다. 단, 단순 문서 수정이나 사용자의 명시적인 요청이 있을 때는 `main` 브랜치에 직접 커밋할 수 있습니다.
- **Pushing**: 원격 `origin`으로의 `push`는 사용자가 요청하거나 명시적으로 승인했을 때만 실행합니다. Never `git push --force` to `main`, and never delete a branch (local or remote) without explicit confirmation, even if it looks merged or stale.
