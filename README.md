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

### 1. Dependencies
- Docker
- Node.js / npm
- Tailwindcss

### 2. Clone repo

1. Clone repo using `git clone https://github.com/Gorilla-Mode/NRL-Obstacle-Reporting.git`

### 3. Build the project
> <details>
> <summary style="font-size: 14px; font-weight: bold">1. Run using terminal (recommended) </summary>
>
>1. Compile CSS 
>   1. Make sure **Node.js** is installed
>   2. Cd to `./NRLObstacleReporting/NRLObstacleReporting` where **package.json** is located
>   3. Run `npm run build:css` to compile tailwind css
>2. Compose environment
>   1. Make sure docker is running `docker desktop start`
>   2. Cd back to `./NRLObstacleReporting` where the **docker-compose.yml** is located
>   3. Run `docker compose up` to launch the application in docker
></details>

> <details>
> <summary style="font-size: 14px">2. Run using build config</summary>
>
>1. Compile CSS
>   1. Make sure **Node.js** is installed
>   2. Cd to `./NRLObstacleReporting/NRLObstacleReporting` where **package.json** is located
>   3. Run `npm run build:css` to compile tailwind css
>2. Add build config in IDE
>   1. Open **NRLObstacleReporting.snl** solution in root folder
>   2. Add build config to run the docker compose 
>   3. Make sure docker is running `docker desktop start`
>   4. Build the solution to launch the application in docker
> </details>

### Possible issues and fixes

   1. Unable to connect to database
      1. The database uses port `3306`, make sure its available
   2. Css won't compile
      1. Make sure npm is initated, `npm install`
      2. Run `npm run build:css` in `./NRLObstacleReporting/NRLObstacleReporting`
## Documentation and System Architecture

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


    
