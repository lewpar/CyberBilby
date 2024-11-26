---
pubDate: 2024-10-11T13:22:42+11:00

author: lewpar

title: Having more than one script in a Svelte component
summary: A component can only have one instance-level <script> element, but how do you fix this?
category: article

heroImage: /blog-content/svelte-component-only-one-instance/hero.png
---

## "A component can only have one instance-level \<script\> element"
While you are learning how to use Svelte you may have run into this error and are scratching your head on what this even means.

When your component is created by Svelte (instantiated) it looks for the script area to execute. This happens on the server when a user requests the page for the first time (server-side rendering). Svelte requires that there is only one script tag in your component so there is no confusion on what script to run.

### What if I want to include external scripts?
You can add the scripts to the page template (app.html), however what if you only want to include those scripts in a single page and not across the entire website?

There is a solution for this: You can import a flag from the app environment that will tell you if you currently running in the browser context or the server context.

## Rendering in the Browser
To import the browser you need to add this import to your script section.
```ts
import { browser } from "$app/environment";
```

To use this flag you can wrap the script tags you want to include like this:
```html
{#if browser}
<script type="module" src="https://cyberbilby.com/static/mod.js"></script>
{/if}
```
This will solve the error by checking if the component is rendering in the browser context, then include the script in the page.