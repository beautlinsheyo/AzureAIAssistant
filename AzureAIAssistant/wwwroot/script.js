
const chatForm = document.getElementById('chatForm');
const userInput = document.getElementById('userInput');
const chatWindow = document.getElementById('chatWindow');
const emptyState = document.getElementById('emptyState');
const sendBtn = document.getElementById('sendBtn');
const clearBtn = document.getElementById('clearBtn');

let conversationHistory = [];

chatForm.addEventListener('submit', async (e) => {
    e.preventDefault();

    const message = userInput.value.trim();
    if (!message) return;

    if (emptyState) emptyState.remove();

    addMessage(message, 'user');
    userInput.value = '';
    sendBtn.disabled = true;

    const loadingEl = addTypingIndicator();

    try {
        const response = await fetch('/api/Assistant', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                userMessage: message,
                history: conversationHistory
            })
        });

        if (!response.ok) {
            throw new Error('Request failed: ' + response.status);
        }

        const data = await response.json();
        loadingEl.remove();
        addMessage(data.aiResponse, 'ai');

        conversationHistory.push({ role: 'user', content: message });
        conversationHistory.push({ role: 'assistant', content: data.aiResponse });

    } catch (err) {
        loadingEl.remove();
        addMessage('Something went wrong. Please try again.', 'ai');
        console.error(err);
    } finally {
        sendBtn.disabled = false;
        userInput.focus();
    }
});

clearBtn.addEventListener('click', () => {
    conversationHistory = [];
    chatWindow.innerHTML = '<div class="empty-state" id="emptyState">Type a message below to start the conversation.</div>';
});

function addMessage(text, cssClass) {
    const wrapper = document.createElement('div');
    wrapper.className = 'message ' + cssClass;

    const textEl = document.createElement('div');
    textEl.textContent = text;
    wrapper.appendChild(textEl);

    const meta = document.createElement('div');
    meta.className = 'message-meta';
    meta.textContent = formatTime(new Date());
    wrapper.appendChild(meta);

    chatWindow.appendChild(wrapper);
    chatWindow.scrollTop = chatWindow.scrollHeight;
    return wrapper;
}

function addTypingIndicator() {
    const wrapper = document.createElement('div');
    wrapper.className = 'message ai';

    const dots = document.createElement('div');
    dots.className = 'typing-dots';
    dots.innerHTML = '<span></span><span></span><span></span>';
    wrapper.appendChild(dots);

    chatWindow.appendChild(wrapper);
    chatWindow.scrollTop = chatWindow.scrollHeight;
    return wrapper;
}

function formatTime(date) {
    return date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
}