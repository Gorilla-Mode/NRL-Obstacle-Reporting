#  **NRL Obstacle Reporting**

## Group 8
| Candidate            | E-mail                              | Github Username                                   |
|----------------------|-------------------------------------|---------------------------------------------------|
| Iver Kroken          | [iverk@uia.no](iverk@uia.no)        | [iverkroken](https://github.com/iverkroken)       |
| Tobias Olsen Nodland | [tobiason@uia.no](tobiason@uia.no ) | [Gorilla-Mode](https://github.com/Gorilla-Mode)   |
| Sivert Svanes Sæstad | [sivertss@uia.no](sivertss@uia.no)  | [sivert-svanes](https://github.com/sivert-svanes) |
| Eira Bitnes Vikstøl  | [eriabv@uia.no](eriabv@uia.no)      | [EiraBV](https://github.com/EiraBV)               |
| Mina Rebecca Remseth | [minarr@uia.no](minarr@uia.no)      | [minaremseth](https://github.com/minaremseth)     |
| Oda Elise Aanestad   | [odaea@uia.no](odaea@uia.no)        | [odaeaanestad](https://github.com/Odaeaanestad)                                  |
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
## Prefixes & naming convention
### Commit message & PR names
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

e.g:
`"feat: added login page"`
### Branch names
- Use the same prefixes as listed above
- Use a slash(/) to seperate prefix and name
- Lowercase only
- Use a **single** hyphen(-) to separate words
- Short and descriptive

e.g:
`refactor/login-page-refactor`

## Working with branches

- Avoid using `git pull` when a `push`is rejected due to the remote branch being ahead of the local; This will create a new branch and merge commit. To avoid this, instead use:
  - `git fetch`
  - `git pull --rebase origin branch-name`
- DON'T REBASE PUSHED COMMITS, IT WILL CHANGE HASHES; IT IS BANNED!!!
![NO REBASING](https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fi.pinimg.com%2Foriginals%2Fb6%2F41%2F32%2Fb6413233b0c147d8e25ac8c6939003ec.jpg&f=1&nofb=1&ipt=e684870cc0f2f939c06bfc53af8ede80336966cf2877b4c2eaeab1dfda026a48)

- - DON'T BRANCH OFF A BRANCH, IT WILL CREATE MERGE CONFLICTS; IT IS FORBIDDEN!!!
![NO REBASING](https://y.yarn.co/9892718e-f9f9-400b-8273-9f5f78e36e22_text.gif)


    
