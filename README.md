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

## Project goal
The main goal of our project is to design and develop a fast, reliable, and user-friendly web application for reporting new or modified aviation obstacles. The system enables pilots to quickly and accurately submit obstacle information, even under time pressure, while providing registrars at Kartverket with an efficient workflow for filtering, reviewing, validating, and publishing reports to the National Register of Aviation Obstacles (NRL).

In addition to core functionality, the project emphasizes modern web technologies and security practices. The user interface is built with HTML to ensure clarity and accessibility, while unit testing has been applied to verify correctness and reliability of the system. To strengthen security, the application has been designed to mitigate common vulnerabilities such as SQL injection, Cross-Site Scripting (XSS), and Cross-Site Request Forgery (CSRF). Furthermore, the system includes registration and login functionality with proper authorization and authentication, ensuring that only pilots and registrars with the correct roles can access their respective features.
## Further documentation, system architecture, testing scenarios and scripts

See the [wiki](https://github.com/Gorilla-Mode/NRL-Obstacle-Reporting/wiki)

## Installation / Setup

> [!CAUTION]
> ### 1. Dependencies
> - Docker
> - Node.js / npm
> - Tailwindcss

### 2. Clone repo

1. Clone repo using `git clone https://github.com/Gorilla-Mode/NRL-Obstacle-Reporting.git`

### 3. Build the project

#### General steps

- Compile CSS
- Generate environment variables
- Deploy containers
- Inject and run SQL on database container

> [!IMPORTANT]
> <details open>
> <summary style="font-size: 14px; font-weight: bold">1. Build using powershell script (recommended) </summary>
>
>   1. Compile CSS 
>      1. Make sure **Node.js** is installed
>      2. Cd to `./NRLObstacleReporting/NRLObstacleReporting` where **package.json** is located
>      3. Initialize npm with:
>           ```powershell
>           npm install
>           ```  
>      4. Compile CSS with:
>           ```powershell
>           npm run build:css
>           ```
>       
> 2. Compose environment
>      1. Make sure docker is running `docker desktop start`
>      2. In **root folder**, run:
>           ```powershell
>            .\build.ps1 -d
>           ```
>          *This will prompt for necessary user input to generate `.env` file. And will deploy containers in detached mode*
>         1. For a clean installation use flag `-c`. See the [Documentation](https://github.com/Gorilla-Mode/NRL-Obstacle-Reporting/wiki/build.ps1) for more info and optional flags
> 3. Inject SQL 
>    1. Make sure port 3306 is available
>    2. in **root folder**, run:
> 
>       ```powershell
>        .\initdb.ps1 -rc -l
>       ``` 
>       
>         *This will inject SQL into container and build the database, restart the containers with logger allowing tests to be confirmed*
>
>       1. See the [Documentation](https://github.com/Gorilla-Mode/NRL-Obstacle-Reporting/wiki/initdb.ps1) for further info about the script
>   3. Confirm that all integration test have succeeded.
>
> Complete information about scripts can be found [here](https://github.com/Gorilla-Mode/NRL-Obstacle-Reporting/wiki#scripts)
></details>



> [!IMPORTANT]
> <details>
> <summary style="font-size: 14px">2. Run using build config</summary>
>
>   1. Compile CSS
>      1. Make sure **Node.js** is installed
>      2. Cd to `./NRLObstacleReporting/NRLObstacleReporting` where **package.json** is located
>      3. Run `npm run build:css` to compile tailwind css
>   2. Make .env file
>      1. in **root folder** run `./build.ps1 -nc`
>         1. See the [Documentation](https://github.com/Gorilla-Mode/NRL-Obstacle-Reporting/wiki/build.ps1) for further
>         info about this script
>   3. Add build config in IDE
>        1. Open **NRLObstacleReporting.snl** solution in root folder
>        2. Add build config to run the docker compose
>        3. Make sure docker is running `docker desktop start`
>        4. Build the solution to launch the application in docker
>   4. Inject SQL
>      1. in **root folder**, run `./initdb.ps1` to inject sql into container and build the database
>         1. See the [Documentation](https://github.com/Gorilla-Mode/NRL-Obstacle-Reporting/wiki/initdb.ps1) for further
>         info about this script
> </details>

> [!IMPORTANT]
> <details>
> <summary style="font-size: 14px">3. Manual install (not recommended)</summary>
>
>   1. Compile CSS
>      1. Make sure **Node.js** is installed
>      2. Cd to `./NRLObstacleReporting/NRLObstacleReporting` where **package.json** is located
>      3. Run `npm run build:css` to compile tailwind css
>   2. Make .env file
>      1. Make`.env` file in **root folder**
>         1. e.g `echo >> .env`, `touch .env`, `cat > .env`
>      2. Populate `.env` file with required fields
>         1. Check the [Documentation](https://github.com/Gorilla-Mode/NRL-Obstacle-Reporting/wiki) for example env file
>   3. Deploy containers
>      1. make sure docker is running
>      2. consider running `docker-compose down -v --rmi "all"` for a clean installation.
>         1. This will remove any existing images, containers and volumes defined in `docker-compose.yml`
>      3. run `docker-compose up` in **root folder**
>   4. Copy SQL into container
>      1. make sure database container is running
>      2. run `docker cp ./db.sql db:/` to copy database sql script into container
>   5. Execute SQL in container
>      1. run ` docker exec db sh -c "mariadb YOURDBNAME -u root -pYOURPASSWORD <db.sql"`
>
> </details>


### Possible issues and fixes

   1. Unable to connect to database
      1. The database uses port `3306`, make sure its available
      2. Do a clean install using `./build.ps1 -c`
   2. Css won't compile
      1. Make sure npm is initated, `npm install`
      2. Run `npm run build:css` in `./NRLObstacleReporting/NRLObstacleReporting`
   3. Site is just plain html
      1. Run `npm run build:css` in `./NRLObstacleReporting/NRLObstacleReporting`

### 4. Default Users

1. Once the application is running use any of the default users to log in as their respecitve role.
   1. To create a new user you must log in as the administrator

| User name               | Default Password | Role          |
|-------------------------|------------------|---------------|
| pilot@pilot.com         | Pilot1.          | Pilot         |
| registrar@registrar.com | Registrar1.      | Registrar     |
| admin@admin.com         | Admin1.          | Administrator |

>[!WARNING]
>
> Passwords must be typed in manually, copy/paste does not work!

## Permanent Branches

>[!WARNING]
> 
> External branches no changes to NRLObstacleReporting directories are to be pushed to these branches. 
> 
> **docs/readme**
> - Any changes to readme branch is done here
>
> **test/unit-test**
> - Unit tests made in the test project should be done here
> - Branch of the branch if necessary
>   - merge to this branch before main
>
> **database/db**
> - Keep only changes to `db.sql`, other sql scripts and injection scripts to this branch
> - Branch of the branch if necessary
>   - merge to this branch before main

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
- database: changes to data transfer objects, repository, and sql 
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

- - DON'T WORK ON THE SAME FEATURE ON DIFFERENT BRANCHES, IT WILL CREATE MERGE CONFLICTS; IT IS FORBIDDEN!!!
![NO REBASING](https://y.yarn.co/9892718e-f9f9-400b-8273-9f5f78e36e22_text.gif)


    
