import { Download, Edit3, MoreHorizontal, Search, SlidersHorizontal, Trash2, X } from 'lucide-react'

export function PageHeader({ eyebrow, title, description, children }) {
  return (
    <div className="page-header">
      <div><span className="eyebrow">{eyebrow}</span><h1>{title}</h1>{description && <p>{description}</p>}</div>
      {children && <div className="page-actions">{children}</div>}
    </div>
  )
}

export function MetricCard({ icon: Icon, label, value, delta, tone = 'maroon' }) {
  return (
    <article className="metric-card">
      <div className={`metric-icon ${tone}`}><Icon size={20} /></div>
      <div className="metric-copy"><span>{label}</span><strong>{value}</strong><small>{delta}</small></div>
    </article>
  )
}

export function StatusPill({ children }) {
  const lower = String(children).toLowerCase()
  const type = lower.includes('active') || lower.includes('submitted') || lower.includes('available') || lower.includes('present') ? 'success' : lower.includes('leave') || lower.includes('progress') || lower.includes('mentoring') ? 'warning' : 'neutral'
  return <span className={`status ${type}`}><i />{children}</span>
}

export function SearchField({ placeholder = 'Search…', value, onChange }) {
  return <label className="search-field"><Search size={17} /><input placeholder={placeholder} value={value ?? ''} onChange={(event) => onChange?.(event.target.value)} /></label>
}

export function DataTable({ columns, rows, onView, onEdit, onDelete }) {
  const hasActions = Boolean(onView || onEdit || onDelete)
  return (
    <div className="table-wrap">
      <table>
        <thead><tr>{columns.map((column) => <th key={column.key}>{column.label}</th>)}{hasActions && <th />}</tr></thead>
        <tbody>{rows.length === 0 ? (
          <tr><td className="empty-table" colSpan={columns.length + (hasActions ? 1 : 0)}>No records available yet.</td></tr>
        ) : rows.map((row) => (
          <tr key={row.id || row.title}>
            {columns.map((column) => <td key={column.key}>{column.render ? column.render(row[column.key], row) : row[column.key]}</td>)}
            {hasActions && <td>
              <div className="row-actions">
                {onEdit && <button className="icon-button" onClick={() => onEdit(row)} aria-label="Edit row"><Edit3 size={17} /></button>}
                {onDelete && <button className="icon-button danger" onClick={() => onDelete(row)} aria-label="Delete row"><Trash2 size={17} /></button>}
                {onView && <button className="icon-button" onClick={() => onView(row)} aria-label="Open row"><MoreHorizontal size={18} /></button>}
              </div>
            </td>}
          </tr>
        ))}</tbody>
      </table>
    </div>
  )
}

export function Toolbar({ search = 'Search records…', searchValue = '', onSearch, onFilter, filterActive = false, onExport, children }) {
  return <div className="toolbar"><SearchField placeholder={search} value={searchValue} onChange={onSearch} /><div className="toolbar__actions"><button type="button" className={filterActive ? 'btn btn-secondary active' : 'btn btn-secondary'} onClick={onFilter}><SlidersHorizontal size={16} /> Filters</button>{onExport && <button type="button" className="btn btn-secondary" onClick={onExport}><Download size={16} /> Export</button>}{children}</div></div>
}

export function Modal({ title, item, onClose, children }) {
  if (!item && !children) return null
  return (
    <div className="modal-backdrop" onMouseDown={onClose}>
      <div className="modal" onMouseDown={(event) => event.stopPropagation()}>
        <button className="modal-close" onClick={onClose}><X size={20} /></button>
        {children ? (
          <>
            <span className="eyebrow">Record details</span>
            <h2>{title}</h2>
            {children}
            <div className="modal-actions"><button className="btn btn-secondary" onClick={onClose}>Close</button></div>
          </>
        ) : (
          <>
        <div className="modal-mark">{String(item.name || item.title).split(' ').map((word) => word[0]).slice(0, 2).join('')}</div>
        <span className="eyebrow">Record details</span>
        <h2>{item.name || item.title}</h2>
        <p>{title} information is shown with prototype data. This panel is ready to connect to a future API.</p>
        <div className="detail-grid">{Object.entries(item).map(([key, value]) => (
          <div key={key}><span>{key.replace(/([A-Z])/g, ' $1')}</span><strong>{String(value)}</strong></div>
        ))}</div>
        <div className="modal-actions"><button className="btn btn-secondary" onClick={onClose}>Close</button><button className="btn btn-primary">Edit record</button></div>
          </>
        )}
      </div>
    </div>
  )
}
