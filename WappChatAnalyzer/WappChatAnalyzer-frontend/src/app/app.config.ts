import { isDevMode } from "@angular/core";

export var appConfig = {
    get apiUrl(): string {
        if (isDevMode())
            return "https://localhost:5001/";
        else
            return "/";
    },
    colors: {
        "Valentino Vukelic": "#16BAC5",
        "Lara <3": "#8B86C1"
    }
};