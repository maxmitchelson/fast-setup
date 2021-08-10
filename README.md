# ⚡ Fast Setup ⚡
This is a small application designed to make the installation process of new Windows PCs faster and easier. 
This project was intended to automate parts of my job as a computer technician in a retail chain.

## Profiles
Although it can be used to install specific apps or to automate specific tasks, this program is primarly made with the concept of `profiles` in mind.
Profiles group multiple installations and tasks together in a single action and can be added manually in the `data/profiles` directory.

To create your own profile, simply create a json file with a list of all app and task IDs to include (see example below.)

NOTE: you can also use `"*"` as a wildcard.

```json
{
    "apps": [
        "*"
    ],
    "tasks": [
        "connect_wifi",
        "sync_time"
    ]
}
```
