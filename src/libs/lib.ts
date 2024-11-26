export function toTitleCase(input: string) {
    return input.charAt(0).toUpperCase() + input.substring(1).toLowerCase();
}

export type Commit = {
    id: string;
    link: string;
    title: string;
    updated: string;
    author: string;
    content: string;
};
  
export async function fetchAndParseCommitsWithRegex(url: string): Promise<Commit[]> {
    const response = await fetch(url);

    if (!response.ok) {
        throw new Error(`Failed to fetch XML: ${response.statusText}`);
    }

    const xmlText = await response.text();

    // Match all entries
    const entryRegex = /<entry>(.*?)<\/entry>/gs;
    const entries = [...xmlText.matchAll(entryRegex)].map(match => match[1]);

    const commits: Commit[] = [];

    for (const entry of entries) {
        // Extract individual fields
        const id = /<id>(.*?)<\/id>/.exec(entry)?.[1] ?? "";
        const link = /<link .*? href="(.*?)"/.exec(entry)?.[1] ?? "";
        const title = /<title>\s*(.*?)\s*<\/title>/.exec(entry)?.[1]?.trim() ?? "";
        const updated = /<updated>(.*?)<\/updated>/.exec(entry)?.[1] ?? "";
        const author = /<author>.*?<name>(.*?)<\/name>.*?<\/author>/s.exec(entry)?.[1] ?? "";
        const content = /<content>(.*?)<\/content>/s.exec(entry)?.[1]?.replace(/&lt;.*?&gt;/g, "").trim() ?? "";

        commits.push({ id, link, title, updated, author, content });
    }

    return commits;
}