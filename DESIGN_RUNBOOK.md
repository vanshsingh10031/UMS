# UMS Design Runbook

Use this checklist before finishing any new portal segment.

## Page Structure

- Use `PageHeader` for every route with one clear eyebrow, title, and description.
- Use operational layouts for portal modules: forms, tables, timelines, dashboards, and lists. Avoid generic placeholder cards for real routes.
- Keep forms inside `.panel.form-panel` and put submit buttons in `.form-actions` so buttons align consistently.
- Use `.panel.data-panel` for tables and always allow the empty state to render.

## Layout Rules

- Do not nest cards inside cards.
- Do not let forms stretch visually without a section heading or purpose.
- Keep repeated items in grids, but collapse to one column on small screens.
- Give fixed-format UI, such as tables, cards, forms, and timelines, stable spacing so browser zoom or long text does not break the layout.
- Do not rely on browser overlays, notifications, or screen recorder UI during review; test with a clean viewport.

## Visual QA

- Check each changed route at desktop width and mobile width.
- Confirm no content is hidden under the sidebar, topbar, or bottom of the viewport.
- Confirm empty tables show a message, not a blank block.
- Confirm buttons are aligned with their form or toolbar.
- Confirm long names, report rows, and select values do not overflow their containers.

## Current Pattern

- Masters: `RecordsPage`
- Operations: focused CRUD/list page
- Academic workflows: `.form-panel` plus `.data-panel`
- Calendar/timeline: `CalendarPage`
- Reports: metrics plus `DataTable`
