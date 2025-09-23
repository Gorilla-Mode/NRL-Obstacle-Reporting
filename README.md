#  **NRL Obstacle Reporting**

## Group 8
| Candidate            | E-mail                              |
|----------------------|-------------------------------------|
| Iver Kroken          | [iverk@uia.no](iverk@uia.no)        |
| Tobias Olsen Nodland | [tobiason@uia.no](tobiason@uia.no ) |
| Sivert Svanes Sæstad | [sivertss@uia.no](sivertss@uia.no)  |
| Eira Bitnes Vikstøl  | [eriabv@uia.no](eriabv@uia.no)      |
| Mina Rebecca Remseth | [minarr@uia.no](minarr@uia.no)      |
| Oda Elise Aanestad   | [odaea@uia.no](odaea@uia.no)        |
## Installation / Setup
### 1. Clone repo
1. Clone repo using `git clone https://github.com/Gorilla-Mode/NRL-Obstacle-Reporting.git`
### 2. Run using build config
1. Open **NRLObstacleReporting.snl** solution
2. Add build config to run the docker compose 
3. Make sure docker is running `docker desktop start`
4. Build the solution to launch the application in docker
### 3. Run using terminal
1. Make sure docker is running `docker desktop start`
2. Cd to `./NRL-Obstacle-Reporting` where the docker-compose.yml is located
3. Run `docker compose up` to launch the application in docker
## Documentation
See the [wiki](https://github.com/Gorilla-Mode/NRL-Obstacle-Reporting/wiki)
## PR/Commit prefixes:
- feat: A new feature
- visual: Changes that only affect frontend visuals, text (html, css)
- fix: A bug fix
- docs: Documentation only changes
- style: Changes that do not affect the meaning of the code (white-space, formatting, missing semicolons, etc.)
- refactor: A code change that neither fixes a bug nor adds a feature
- perf: A code change that improves performance
- test: Adding missing tests or correcting existing tests
- build: Changes that affect the build system or external dependencies (docker, libraries, etc.)
- chore: Other changes that don't modify src or test files
- revert: Reverts a previous commit

These prefixes should also be used on branches e.g:

`feat/loginauth`