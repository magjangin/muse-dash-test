# Project Instructions

- If a source file grows beyond 500 lines, automatically refactor it into smaller, focused units before adding more substantial logic.
- Keep refactors behavior-preserving unless the user explicitly asks for behavior changes.
- After changes, run `dotnet build` when practical.
- After completing work, always create a git commit.
- Before starting any work, run `git fetch origin` and check `git status -sb` for `ahead/behind` against `origin/main`. This repo is worked on from multiple clones/sessions, and unsynced starts have caused real divergence before (duplicate reimplementation of the same feature, docs reorganized into two different folder schemes on the same day). If behind, pull/rebase before making changes. If diverged (both ahead and behind), stop and surface it instead of guessing which side is authoritative — that call is the user's.
- Never `git push --force` to `main`, and never delete a branch (local or remote) without explicit confirmation, even if it looks merged or stale.
